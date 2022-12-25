using System;
using UnityEngine;

namespace Better.RuntimeConsole.Runtime.ConsoleElements
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
