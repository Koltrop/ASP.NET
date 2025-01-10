namespace PromoCodeFactory.DataAccess;

public static class DbConstraints
{
    public static class MaxLength
    {
        public const int Code = 50;

        public const int Name = 200;

        public const int Text = 200;

        public const int Email = 100;
    }
}
