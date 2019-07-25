﻿/*
 * Copyright (c) 2010-2012, Achim 'ahzf' Friedland <code@ahzf.de>
 * This file is part of Pipes.NET <http://www.github.com/ahzf/Pipes.NET>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace de.ahzf.Styx
{

    /// <summary>
    /// Emits the path that the traverser has taken up to this object.
    /// In other words, it uses the Path property of the previous pipe
    /// to emit the transformation stages.
    /// </summary>
    /// <typeparam name="S">The type of the consuming objects.</typeparam>
    public class PathPipe<S> : AbstractPipe<S, IEnumerable<Object>>, IEndPipe<IEnumerable<Object>>
    {

        #region MoveNext()

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// True if the enumerator was successfully advanced to the next
        /// element; false if the enumerator has passed the end of the
        /// collection.
        /// </returns>
        public override Boolean MoveNext()
        {

            if (_InternalEnumerator == null)
                return false;

            if (_InternalEnumerator is IPipe)
            {
                
                if (_InternalEnumerator.MoveNext())
                {
                    _CurrentElement = ((IPipe) _InternalEnumerator).Path;
                    return true;
                }
                
                return false;

            }
            
            else
                throw new NoSuchElementException("The source of this pipe was not a pipe!");

        }

        #endregion

    }

}
