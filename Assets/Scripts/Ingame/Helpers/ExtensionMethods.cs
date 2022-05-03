namespace Warborn.Ingame.Helpers
{
    public static class ExtensionMethods
    {
        public static float Remap(this float _value, float _from1, float _to1, float _from2, float _to2)
        {
            return (_value - _from1) / (_to1 - _from1) * (_to2 - _from2) + _from2;
        }

        public static int Remap(this int _value, int _from1, int _to1, int _from2, int _to2)
        {
            return (_value - _from1) / (_to1 - _from1) * (_to2 - _from2) + _from2;
        }
    }
}

