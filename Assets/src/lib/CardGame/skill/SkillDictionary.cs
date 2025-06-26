using System.Collections.Generic;

public static class SkillDictionary { 
    static public SkillBundle Get(Card ownerCard) {
        Dictionary<string, SkillBundle> Dictionary = new() {
            {"bird", new Bird()},
            {"fish", new Fish()},
            {"shark", new Shark()},
            {"test", new Test()},
            {"bush", new Bush()},
            {"oldTree", new OldTree()},
            {"snake", new Snake()},
            {"lion", new Lion()},
            {"scarecrow", new Scarecrow()},
        };
        SkillBundle result = Dictionary[ownerCard.spec.id];
        result.Init(ownerCard);
        return result;
    }
}