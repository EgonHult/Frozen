using System;

namespace Users.UnitTest.Context
{
    internal class AddIdentity<T1, T2>
    {
        private Action<object> p;

        public AddIdentity(Action<object> p)
        {
            this.p = p;
        }
    }
}