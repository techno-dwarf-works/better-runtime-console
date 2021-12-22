#region license

// Copyright 2021 - 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using UnityEngine;

namespace UnityConsole.Scripts.ConsoleElements
{
    /// <summary>
    /// List of icons for <seealso cref="RuntimeConsole"/>
    /// </summary>
    public sealed class ConsoleIcons : ScriptableObject
    {
        [SerializeField] private Sprite infoActive;
        [SerializeField] private Sprite infoInactive;

        [SerializeField] private Sprite warningActive;
        [SerializeField] private Sprite warningInactive;

        [SerializeField] private Sprite errorActive;
        [SerializeField] private Sprite errorInactive;

        public Sprite InfoActive => infoActive;
        public Sprite InfoInactive => infoInactive;

        public Sprite ErrorActive => errorActive;
        public Sprite ErrorInactive => errorInactive;

        public Sprite WarningActive => warningActive;
        public Sprite WarningInactive => warningInactive;

        public Sprite GetLogIconSprite(LogType type, bool active)
        {
            var sprite = type switch
                         {
                             LogType.Exception => active ? errorActive : errorInactive,
                             LogType.Assert => active ? errorActive : errorInactive,
                             LogType.Error => active ? errorActive : errorInactive,
                             LogType.Warning => active ? warningActive : warningInactive,
                             LogType.Log => active ? infoActive : infoInactive,
                             _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                         };
            return sprite;
        }
    }
}
