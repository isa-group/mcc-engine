using System.Collections.Generic;
using de.ahzf.Styx;
using System.Diagnostics;
using isa.MCC.Mashups;
using isa.MCC.Pipes.Generic;

namespace isa.MCC.Controls
{
    public class ControlResultWithContext : IControlContextEvaluation
    {
        public string Name { get; }
        public string Description { get; }
        public ControlInformation Information { get; }

        private IEnumerable<MashupConfiguration> _mashups;

        public ControlResultWithContext(string name, string description, ControlInformation info, IEnumerable<MashupConfiguration> mashups)
        {
            _mashups = mashups;
            Information = info;
            Name = name;
            Description = description;
        }


        public List<ControlContextEvaluation> Evaluate()
        {
            List<ControlContextEvaluation> result;

            Debug.WriteLine("--------------------------------------------------");
            Debug.WriteLine("Evaluating control " + Name, "ControlContext");
            Debug.Indent();

            IEnumerable<List<ControlContextEvaluation>> evaluations = _mashups.MapEach(m => InternalEvaluate(m));

            Debug.Unindent();

            result = MergeEvaluations(evaluations);
            Debug.WriteLine("Evaluations merged successfully", "ControlContext");

            Debug.WriteLine("--------------------------------------------------");

            return result;

        }

        private List<ControlContextEvaluation> InternalEvaluate(MashupConfiguration c)
        {
            List<ControlContextEvaluation> result = new List<ControlContextEvaluation>();
            MashupContainer container = c.Build();
            IPipe pipe = container.Pipe;

            while (pipe.MoveNext())
            {
                ResultWithContext eval = pipe.Current as ResultWithContext;

                if (eval != null)
                {
                    Debug.Unindent();
                    Debug.WriteLine(string.Format("Evaluating Context {0} with result {1}", eval.Context, eval.Result), "ControlContext");

                    bool boolEval = false;
                    if (eval.Result is bool b)
                    {
                        boolEval = b;
                    }

                    IContext mergedContext = Information.Context.Merge(eval.Context);
                    ControlContextEvaluation evaluation = new ControlContextEvaluation(mergedContext, boolEval);

                    result.Add(evaluation);

                    Debug.WriteLine("Evaluation: " + evaluation, "ControlContext");
                    Debug.Indent();
                }
                Debug.WriteLineIf(eval == null, "Evaluation is null", "ControlContext");
            }

            return result;
        }

        private List<ControlContextEvaluation> MergeEvaluations(IEnumerable<List<ControlContextEvaluation>> evaluations)
        {
            List<ControlContextEvaluation> result;
            IDictionary<IContext, ControlContextEvaluation> dic = new SortedDictionary<IContext, ControlContextEvaluation>();

            foreach (List<ControlContextEvaluation> eval in evaluations)
            {
                foreach (ControlContextEvaluation p in eval)
                {
                    if (dic.ContainsKey(p.Context))
                    {
                        dic[p.Context] = MergeContextEvaluation(dic[p.Context], p);
                    }
                    else
                    {
                        dic.Add(p.Context, p);
                    }
                }
            }

            result = new List<ControlContextEvaluation>(dic.Values);
            return result;
        }

        private ControlContextEvaluation MergeContextEvaluation(ControlContextEvaluation evaluation, ControlContextEvaluation p)
        {
            evaluation.Evaluation = evaluation.Evaluation && p.Evaluation;
            return evaluation;
        }
    }
}
