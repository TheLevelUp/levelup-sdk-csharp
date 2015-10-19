
using System;

namespace LevelUp.Api.Http
{
    public class LevelUpEnvironment
    {
        public static LevelUpEnvironment Production
        {
            get { return new LevelUpEnvironment(@"https://api.thelevelup.com"); }
        }

        public static LevelUpEnvironment Sandbox
        {
            get { return new LevelUpEnvironment(@"https://sandbox.thelevelup.com"); }
        }

        public static LevelUpEnvironment Staging
        {
            get { return new LevelUpEnvironment(@"https://staging.thelevelup.com"); }
        }

        private readonly string _baseUri;

        private LevelUpEnvironment(string baseUri)
        {
            _baseUri = baseUri;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LevelUpEnvironment))
            {
                return false;
            }

            return Equals(obj as LevelUpEnvironment);
        }

        protected bool Equals(LevelUpEnvironment other)
        {
            return string.Equals(_baseUri, other._baseUri, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (_baseUri != null ? _baseUri.GetHashCode() : 0);
        }

        public static bool operator ==(LevelUpEnvironment left, LevelUpEnvironment right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LevelUpEnvironment left, LevelUpEnvironment right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return _baseUri;
        }
    }
}
