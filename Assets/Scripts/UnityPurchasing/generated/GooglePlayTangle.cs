// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("NQmdxpJyeJp5X7EZIySwU449Xj9INUgkJ0RXpjwwcc7T2Fzn2mdoEuxbKsC4zRMqsAJomvkNrOxn9Esili0dhKNGKUvXS8eF5Y+ir+I8xmQf+kI0PzJTbEIbumS4HvsXxzK1ngw03LQqSe/dQKeFz1nia/5S+tNyZNZVdmRZUl1+0hzSo1lVVVVRVFdvc3AuSvgJQfmtapmZWsx9Mn4f9FQ2aN8p0j6b0lcSax3GrBKTa4r8L+lf+E11g9H9AipJCWW64xobn0QdvWjvAz7cCnI9jPcvlc5Rs1InCrj3tdWafkFlZA9KavwHGh+e58AP1lVbVGTWVV5W1lVVVJcMl8fZfOEyiiiPkhdGCggn6VtIW0tQNMttN3CpV+DTyCntFVZXVVRV");
        private static int[] order = new int[] { 13,9,8,7,8,7,13,12,10,10,11,12,12,13,14 };
        private static int key = 84;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
