using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


using de.ahzf.Styx;
using icinetic.BPMMetamodel;
using System.Diagnostics;

namespace Mashups
{
    //public class MashupElement
    //{
    //    public string Stereotype {get; set;}
    //    public IDictionary<string, string> TagNames {get; set;}
    //    public IList<MashupElement> Next { get; set; }

    //    public MashupElement(string stereotype)            
    //    {
    //        Stereotype = stereotype;
    //        Next = new List<MashupElement>();
    //        TagNames = new Dictionary<string, string>();
    //    }

    //    public MashupElement(string stereotype, MashupElement nextElement) : this(stereotype)
    //    {
    //        Next.Add(nextElement);
    //    }

    //    public MashupElement(string stereotype, IList<MashupElement> nextElements) : this(stereotype)
    //    {
    //        foreach (MashupElement e in nextElements)
    //        {
    //            Next.Add(e);
    //        }
    //    }
    //}


    public class MashupBuilder
    {
        private IList<MashupElement> _roots;
        private IDictionary<MashupElement, PipeType> _types;
        private IDictionary<MashupElement, List<MashupElement>> _inputs;
        private IDictionary<MashupElement, IPipe> _pipes;
        private IDictionary<MashupElement, IDataSource> _dataSources;
        private IDictionary<string, string> _parameters;
        private MashupElement lastElement;

        public MashupBuilder(IEnumerable<MashupElement> roots)
        {
            _roots = new List<MashupElement>(roots);
            _types = new Dictionary<MashupElement, PipeType>();
            _inputs = new Dictionary<MashupElement, List<MashupElement>>();

            _pipes = new Dictionary<MashupElement, IPipe>();
            _dataSources = new Dictionary<MashupElement, IDataSource>();

            _parameters = new Dictionary<string, string>();
        }

        public MashupContainer Build()
        {
            return Build(new Dictionary<string, string>());
        }

        public MashupContainer Build(IDictionary<string, string> parameters)
        {
            Debug.WriteLine("Starts building mashup", "MashupBuilder");
            Debug.Indent();

            Debug.WriteLine("Clearing everything", "MashupBuilder");
            ClearEverything();
            _parameters = parameters;

            Debug.WriteLine("Building dictionaries", "MashupBuilder");
            BuildDictionaries();

            Debug.WriteLine("Instantiating elements", "MashupBuilder");
            InstantiateElements();

            Debug.WriteLine("Linking pipes", "MashupBuilder");
            LinkPipes();

            Debug.WriteLine("Linking roots", "MashupBuilder");
            LinkRoots();

            Debug.Unindent();

            return new MashupContainer(_pipes[lastElement], _dataSources.Values.ToArray());
        }

        private void ClearEverything()
        {
            _types.Clear();
            _inputs.Clear();
            _pipes.Clear();
            _dataSources.Clear();
            lastElement = null;
        }

        # region BuildDictionaries()

        private void BuildDictionaries()
        {
            foreach (MashupElement elem in _roots)
            {
                ProcessElementToDictionary(elem, null);
            }

            //WriteDebugBuildDictionaries();
        }

        private void WriteDebugBuildDictionaries()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ roots: [ ");
            _roots.ForEach(mashupElement => sb.Append(mashupElement.Stereotype+", "));
            sb.Append(" ] }");
            Debug.WriteLine("BuildDictionaries: " + sb.ToString(), "MashupBuilder");
            
            sb = new StringBuilder();
            sb.Append("{ types: [ ");
            _types.Keys.ForEach(key => sb.AppendFormat(" (stereotype: {0}, tagNames: [ {1} ], pipetype: {2}), ", key.Stereotype, key.TagNames, _types[key].Type));
            sb.Append(" ] }");
            Debug.WriteLine("BuildDictionaries: " + sb.ToString(), "MashupBuilder");

            sb = new StringBuilder();
            sb.Append("{ inputs: [ ");
            _inputs.Keys.ForEach(key => sb.AppendFormat(" (stereotype: {0}, inputs: [ {1} ]), ", key.Stereotype, _inputs[key]));
            sb.Append(" ] }");
            Debug.WriteLine("BuildDictionaries: " + sb.ToString(), "MashupBuilder");
        }


        private void ProcessElementToDictionary(MashupElement current, Type currentType)
        {
            if (current == null)
                return;
            else
            {
                if (!_types.ContainsKey(current))
                {
                    PipeType pipeType = new PipeType(current.Stereotype, currentType);

                    if (current.Next.Count > 1 && !pipeType.IsSplitPipe())
                        throw new InvalidOperationException("The implementation of pipe " + current.Stereotype + " is not a split");

                    _types.Add(current, pipeType);

                    if (current.Next.Count == 0)
                    {
                        lastElement = current;
                    }
                    else
                    {
                        currentType = pipeType.GetOutputType();
                        foreach (MashupElement next in current.Next)
                        {
                            if (!_inputs.ContainsKey(next))
                                _inputs.Add(next, new List<MashupElement>());
                            _inputs[next].Add(current);

                            ProcessElementToDictionary(next, currentType);
                        }
                    }
                }
                else
                {
                    if (! _types[current].IsMergePipe())
                        throw new InvalidOperationException("The implementation of pipe " + current.Stereotype + " is not a merge");
                }
            }
        }

        # endregion 

        # region InstantiateElements()

        private void InstantiateElements()
        {
            foreach (MashupElement elem in _types.Keys)
            {
                if (_roots.Contains(elem))
                {
                    _dataSources.Add(elem, BuildDataSource(elem, _types[elem]));
                }
                else
                { 
                    _pipes.Add(elem, BuildPipe(elem, _types[elem]));
                }
            }
        }

        private IDataSource BuildDataSource(MashupElement elem, PipeType pipeType)
        {
            IDataSource pipe = null;
            try
            {
                Type t = pipeType.Type;
                ConstructorInfo info = t.GetConstructor(buildAllStringType(elem.TagNames.Count));
                ParameterInfo[] pinfo = info.GetParameters();

                pipe = (IDataSource)info.Invoke(matchParameters(elem.TagNames, pinfo));
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Error creating data source for stereotype {0} with PipeType {1}", elem.Stereotype, pipeType.Type);
                sb.Append(" and TagNames ");
                elem.TagNames.Keys.ForEach(key => sb.AppendFormat("( {0}: {1} ), ", key, elem.TagNames[key]));
                Debug.WriteLine(sb.ToString(), "MashupBuilder");
                Debug.WriteLine(e, "MashupBuilder");

                throw new InvalidOperationException(sb.ToString(), e);
            }

            return pipe;
        }


        private IPipe BuildPipe(MashupElement elem, PipeType pipeType)
        {
            IPipe pipe = null;

            try
            {
                Type t = pipeType.Type;
                ConstructorInfo info = t.GetConstructor(buildAllStringType(elem.TagNames.Count));
                ParameterInfo[] pinfo = info.GetParameters();

                pipe = (IPipe)info.Invoke(matchParameters(elem.TagNames, pinfo));

                IMashupCaller mc = pipe as IMashupCaller;
                if (mc != null)
                    mc.SetParameters(_parameters);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Error creating pipe for stereotype {0} with PipeType {1}", elem.Stereotype, pipeType.Type);
                sb.Append(" and TagNames ");
                elem.TagNames.Keys.ForEach(key => sb.AppendFormat("( {0}: {1} ), ", key, elem.TagNames[key]));
                Debug.WriteLine(sb.ToString(), "MashupBuilder");
                Debug.WriteLine(e, "MashupBuilder");

                throw new InvalidOperationException(sb.ToString(), e);

            }

            return pipe;
        }

        private Type[] buildAllStringType(int number)
        {
            Type[] result = new Type[number];
            for (int i = 0; i < number; i++)
            {
                result[i] = typeof(string);
            }

            return result;
        }

  
        private object[] matchParameters(IDictionary<string, string> iDictionary, ParameterInfo[] pinfo)
        {
            object[] result = new object[pinfo.Length];

            foreach (ParameterInfo param in pinfo)
            {
                string value;
                if (iDictionary.ContainsKey(param.Name))
                {
                    value = processParameterValue(param.Name, iDictionary[param.Name]);
                }
                else
                {
                    value = "";
                }

                result[param.Position] = value;
            }

            return result;
        }

        private string processParameterValue(string name, string value)
        {
            string result = value;

            if (value.StartsWith("@Param"))
            {
                string[] splitted = value.Split('(', ')');
                string paramName = splitted[1].Trim();
                if (_parameters.ContainsKey(paramName))
                {
                    result = _parameters[paramName];
                }
                else
                {
                    throw new ArgumentException("Not parameter provided for " + name);
                }

            }

            return result;
        }

        # endregion

        # region LinkElements()

        private void LinkPipes()
        {
            foreach (MashupElement elem in _inputs.Keys)
            {
                List<MashupElement> inputs = _inputs[elem];

                if (inputs.Count > 1)
                {
                    LinkForMergePipe(elem, inputs);
                }
                else
                {
                    MashupElement parent = inputs[0];
                    if (parent.Next.Count > 1)
                    {
                        // Do nothing. Link goes in parent
                    }
                    else
                    {
                        LinkForNormalPipe(elem, inputs);
                    }
                }

                if (elem.Next.Count > 1)
                {
                    LinkForSplitPipe(elem, elem.Next);
                }
                
                
            }
        }

        private void LinkForNormalPipe(MashupElement elem, List<MashupElement> inputs)
        {
            if (_pipes.ContainsKey(inputs[0]))
                _pipes[elem].SetSourceCollection(_pipes[inputs[0]]);
            else
                _pipes[elem].SetSourceCollection(_dataSources[inputs[0]]);
        }

        private void LinkForMergePipe(MashupElement elem, List<MashupElement> inputs)
        {
            // TODO: Add support for merge with several input types
            IMergePipe p = (IMergePipe)_pipes[elem];
            p.SetInputPipes(inputs.MapEach(delegate(MashupElement e) { return _pipes[e]; }));
        }

        private void LinkForSplitPipe(MashupElement elem, IEnumerable<MashupElement> outputs)
        {
            ISplitPipe p = (ISplitPipe)_pipes[elem];
            p.SetOutputPipes(outputs.MapEach(delegate(MashupElement e) { return _pipes[e]; }));
        }

        private void LinkRoots()
        {

        }

        # endregion
    }

    public class PipeType
    {
        public Type Type { get { return _pipeType; } }
        private Type _pipeType;

        public PipeType(string className)
        {
            // There are two assemblies in which we can find pipes:
            _pipeType = loadTypeFromAssembly("icinetic.BPCMSPipes", className);
            if (_pipeType == null)
                _pipeType = loadTypeFromAssembly("Styx", className);
        }

        public PipeType(string className, params Type[] genericType)
            : this(className)
        {
            if (IsGenericPipe())
                _pipeType = _pipeType.MakeGenericType(genericType);
        }


        private Type loadTypeFromAssembly(string assemblyName, string className)
        {
            Type result = null;
            Assembly assembly = Assembly.Load(assemblyName);
            Type[] types = assembly.GetTypes();
            foreach (Type t in types)
            {
                string[] leftpart = t.Name.Split('`');
                if (leftpart[0].ToUpper().Equals(className.ToUpper()))
                {
                    result = t;
                    break;
                }
            }

            return result;
        }

        public Type GetOutputType()
        {
            Type endPipeType = _pipeType.GetInterface(typeof(IEndPipe<>).Name);
            // There is only one generic argument in end pipe
            return endPipeType.GetGenericArguments()[0];
        }

        public bool IsMergePipe()
        {
            Type mergePipe = _pipeType.GetInterface(typeof(IMergePipe<,>).Name);
            return mergePipe != null;
        }

        public bool IsSplitPipe()
        {
            Type splitPipe = _pipeType.GetInterface(typeof(ISplitPipe<,>).Name);
            return splitPipe != null;
        }

        public int GetNumberOfInputs()
        {
            if (_pipeType == null)
                return 0;

            Type tpipe = _pipeType.GetInterface(typeof(IStartPipe<>).Name);
            if (tpipe != null)
                return 1;

            tpipe = _pipeType.GetInterface(typeof(IStartPipe<,>).Name);
            if (tpipe != null)
                return 2;

            tpipe = _pipeType.GetInterface(typeof(IStartPipe<,,>).Name);
            if (tpipe != null)
                return 3;

            return 0;
        }

        public bool IsGenericPipe()
        {
            bool isGeneric = false;
            Type tpipe = _pipeType.GetInterface(typeof(IStartPipe<>).Name);

            if (tpipe != null)
            {
                foreach (Type tipo in tpipe.GetGenericArguments())
                {
                    isGeneric = isGeneric || tipo.IsGenericParameter;
                }

            }

            return isGeneric;
        }

    }

}
