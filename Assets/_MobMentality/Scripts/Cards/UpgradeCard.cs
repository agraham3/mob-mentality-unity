namespace MobMentality.Cards
{
    /// <summary>Defines one immutable upgrade choice.</summary>
    public sealed class UpgradeCard
    {
        public UpgradeCard(string name, string description, UpgradeEffect effect, float amount, UpgradeTarget target, CardRarity rarity)
        {
            Name = name;
            Description = description;
            Effect = effect;
            Amount = amount;
            Target = target;
            Rarity = rarity;
        }

        public string Name { get; }
        public string Description { get; }
        public UpgradeEffect Effect { get; }
        public float Amount { get; }
        public UpgradeTarget Target { get; }
        public CardRarity Rarity { get; }
    }
}
