using AutoFixture;
using AutoFixture.AutoMoq;

namespace Tests.Utils
{
    public class DefaultCustomization : CompositeCustomization
    {
        public DefaultCustomization()
            : base(new AutoMoqCustomization())
        { }
    }
}