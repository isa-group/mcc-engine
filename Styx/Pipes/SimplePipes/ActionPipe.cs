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
    /// The ActionPipe is much like the IdentityPipe, but calls
    /// an Action &lt;S&gt; on every consuming object before
    /// returing them.
    /// </summary>
    /// <typeparam name="S">The type of the consuming objects.</typeparam>
    public class ActionPipe<S> : AbstractPipe<S, S>
    {

        #region Data

        private Action<S> _Action;

        #endregion

        #region Constructor(s)

        #region ActionPipe(myAction)

        /// <summary>
        /// Creates a new ActionPipe using the given Action&lt;S&gt;.
        /// </summary>
        /// <param name="myAction">An Action&lt;S&gt; to be called on every consuming object.</param>
        public ActionPipe(Action<S> myAction)
        {

            if (myAction == null)
                throw new ArgumentNullException("myAction must not be null!");

            _Action = myAction;

        }

        #endregion

        #endregion


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

            if (_InternalEnumerator.MoveNext())
            {
                _CurrentElement = _InternalEnumerator.Current;
                _Action(_CurrentElement);
                return true;
            }

            return false;

        }

        #endregion

    }

}
