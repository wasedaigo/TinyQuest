using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Skills;

public class SkillFactory {
	public static readonly SkillFactory Instance = new SkillFactory();
	public Dictionary<int, BaseSkill> skillDictionary;
	
	public SkillFactory() {
		this.skillDictionary = new Dictionary<int, BaseSkill> {
			{1, new DoubleSlash(1)},
			{2, new Fireball(2)},
			{3, new MassiveAttack(3)},
			{4, new PoisonousBlow(4)},
			{5, new DeathParade(5)}
		};
	}
	
	public BaseSkill GetSkill(int skillId) {
		return this.skillDictionary[skillId];
	}
	
	public BaseSkill GetNormalAttack(string animation) {
		return new NormalAttackSkill(animation);
	}
}
