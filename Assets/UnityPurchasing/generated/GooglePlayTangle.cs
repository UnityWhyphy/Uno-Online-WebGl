// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("BVUx9ug7qXa9V4fbV6u80g78CQvAht/wKNgfXk5zzxj73YyNejHNN1PQ3tHhU9Db01PQ0NEbcZFlrhEO79xC1VCl/U8rA4YQbPd+UUGJH5vTjHkPwkmvrma9+FB/hmPf3hA3UVL5lT7p6vunwqKcZK942r4ng/Wqpa7SstdZ4NsK+niSToF9//Mcb1XhU9Dz4dzX2PtXmVcm3NDQ0NTR0musjQ1IR0p7ufqF127dM+fTX/l6xDIrSOH5uDOMg90tR2RtMiQEWvfld/WDbSOKClnNB1y6QywsodPpd9mYD7Pe1m4aTIXKH8gaM68Nh8jn16OFW/FnEX41auqQjPiOppU5VwkICMegPmf/gHb0P6Ax4zmkDFLRHCX7JD/yrPUgbtPS0NHQ");
        private static int[] order = new int[] { 11,6,6,7,7,6,7,11,8,11,11,11,13,13,14 };
        private static int key = 209;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
