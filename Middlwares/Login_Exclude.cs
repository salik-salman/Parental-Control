namespace Parental_Control.Middlwares
{
    public class Login_Exclude
    {
        public Dictionary<string, string> Excludes = new Dictionary<string, string>();
        public IDictionary<string, string> Login_Excludes()
        {
            /**
             * For Specific Action Add Controller Name As Key And Action As Value.
             */

            Excludes.Add("Home", "Index");
            Excludes.Add("Auth", "Auth");
            /**
             * For Complete Controller Add Same Key And Value.
             */
            Excludes.Add("Users", "Users");
            return Excludes;
        }
    }
}
