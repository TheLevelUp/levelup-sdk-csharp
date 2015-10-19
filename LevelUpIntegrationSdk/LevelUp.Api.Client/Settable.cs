//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Settable.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
// </copyright>
// <license publisher="Apache Software Foundation" date="January 2004" version="2.0">
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </license>
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

using System;

namespace LevelUp.Api.Client
{
    /// <summary>
    /// Interface for the Settable type to allow access to the HasValue and Value properties without having to know
    /// the underling type of T
    /// </summary>
    internal interface ISettable
    {
        bool HasValue { get; }
        object Value { get; }
    }

    /// <summary>
    /// Class used to determine if a value has been set or not.  Useful in issuing updates to end points where only
    /// the values that are to be updated are included.
    /// </summary>
    internal struct Settable<T> : ISettable
    {
        private readonly bool _hasValue;
        private readonly T _value;

        private Settable(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public bool HasValue { get { return _hasValue; } }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException("The Settable does not have a value.");
                }

                return _value;
            }
        }

        object ISettable.Value 
        {
            get { return Value; }
        }

        public T GetValueOrDefault(T defaultValue = default(T))
        {
            return HasValue ? _value : defaultValue;
        }

        public static implicit operator Settable<T>(T value)
        {
            return new Settable<T>(value);
        }

        public static explicit operator T(Settable<T> value)
        {
            return value.Value;
        }
    }
}
