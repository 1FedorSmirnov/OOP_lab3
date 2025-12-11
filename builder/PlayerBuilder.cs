using Lab3GameInventory.domain;
using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.builder;

public class PlayerBuilder
{
    private string _name = "NoNameHero";
    private int _baseStrength = 10;
    private int _baseIntelligence = 10;
    private int _baseAgility = 10;
    private float _basePhysicalDamage = 10f;
    private float _baseMagicDamage = 5f;
    private int _baseMagicResistance = 10;
    private int _basePhysicalDefense = 5;
    private ISetService _setService;

    public PlayerBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PlayerBuilder WithBaseStats(int strength, int intelligence, int agility)
    {
        _baseStrength = strength;
        _baseIntelligence = intelligence;
        _baseAgility = agility;
        return this;
    }

    public PlayerBuilder WithBasePhysicalDamage(float physicalDamage)
    {
        _basePhysicalDamage = physicalDamage;
        return this;
    }

    public PlayerBuilder WithBaseMagicDamage(float magicDamage)
    {
        _baseMagicDamage = magicDamage;
        return this;
    }

    public PlayerBuilder WithBaseMagicResistance(int magicResistance)
    {
        _baseMagicResistance = magicResistance;
        return this;
    }

    public PlayerBuilder WithBasePhysicalDefence(int physicalDefence)
    {
        _basePhysicalDefense = physicalDefence;
        return this;
    }
    
    public PlayerBuilder WithSetBonusService(ISetService setBonusService)
    {
        _setService = setBonusService ?? throw new ArgumentNullException(nameof(setBonusService));
        return this;
    }

    public Player Build()
    {
        
        return new Player(
            name: _name,
            baseStrength: _baseStrength,
            baseIntelligence: _baseIntelligence,
            baseAgility: _baseAgility,
            basePhysicalDefense: _basePhysicalDefense,
            baseMagicResistance: _baseMagicResistance,
            basePhysicalDamage: _basePhysicalDamage,
            baseMagicDamage: _baseMagicDamage,
            setBonusService: _setService);
    }

}