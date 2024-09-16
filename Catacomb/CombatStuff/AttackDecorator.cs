using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    //use this class when you need to change the Attack itself.
    //EX: When you want to do 1.1x damage
    //This class is called from the bottom up i.e. baseAttack -> firstDecorator-> seconddecorator ->... -> top layer
    public class AttackDecorator:Attack
    {
        private Attack layerUpAttack; // this is the AttackDecorator or attack that is the next layer up
        public Attack LayerUpAttack { get => layerUpAttack; set => layerUpAttack = value; }

        private Attack mainAttack; //this is the main attack that is being decorated. Does not need to be known when created, but REQURIED when running

        public int priority = 10;
        public delegate double SetStatsDelegate(double value, Attack mainAttack);
        public delegate double InflictDamageDelegate(double value, Attack mainAttack);


        private SetStatsDelegate setDamage = null; //the damage the attack does (+:reduces health)
        private SetStatsDelegate setSelfHeal= null; //how much the one is attacking will heal (negative value will cause self damage)
        private SetStatsDelegate setPoison= null; //how much the attack will poison its target (+: increases poison)
        private SetStatsDelegate setBurn = null;
        private SetStatsDelegate setMentalBreak= null; //how much mentalbreak the attack will apply to the target (+: increases mental break)
        private SetStatsDelegate setArmorChange= null; //how much will the attack change the target armor after the damage is done (+: reduces armor)
        private SetStatsDelegate setSelfArmorChange= null; //how much the attack will change the armor of the user (+: reduces armor)
        private SetStatsDelegate setDefenseStatChange= null; //how much the attack will change the defense value of its target (+:reduces)
        private SetStatsDelegate setMagicResistStatChange= null;//how much the attack will change the magic resist of its target(+: reduces)
        private SetStatsDelegate setSelfMagicResistStatChange= null; //(+: reduces)
        private SetStatsDelegate setSelfDefenseStatChange= null; //how much the attack will change the defense of the user (+: reduces)
        private SetStatsDelegate setAttackStatChange= null; //how much the attack will change the attack value of its target (+: reduces)
        private SetStatsDelegate setSelfAttackStatChange= null; //how much the attack will change the attack value of the user (+: reduces)
        private SetStatsDelegate setMagicAttackStatChange= null; // (+: reduces )
        private SetStatsDelegate setSelfMagicAttackStatChange= null; //(+: reduces)
        private SetStatsDelegate setSpeedStatChange= null;//changes the speed stat of the target (+: reduces)
        private SetStatsDelegate setSelfSpeedStatChange= null; //(+: reduces)
        private SetStatsDelegate setManaDrain= null;
        private SetStatsDelegate setSelfManaDrain= null;

        private InflictDamageDelegate setInflictDamage= null;
        private InflictDamageDelegate setCalculateDamage = null;
        public AttackDecorator(Attack layerUpAttack, Command parent) : base(parent)
        {
            this.layerUpAttack = layerUpAttack;
            setDamage = (double  value, Attack mainAttack) =>value;
            setSelfHeal = (double  value, Attack mainAttack) => value; 
            setPoison = (double  value, Attack mainAttack) => value; 
            setBurn = (double value, Attack mainAttack) => value;
            setMentalBreak = (double  value, Attack mainAttack) =>value; 
            setArmorChange = (double  value, Attack mainAttack) => value; 
            setSelfArmorChange = (double  value, Attack mainAttack) =>value; 
            setDefenseStatChange = (double  value, Attack mainAttack) => value; 
            setMagicResistStatChange = (double  value, Attack mainAttack) => value;
            setSelfMagicResistStatChange = (double  value, Attack mainAttack) => value; 
            setSelfDefenseStatChange = (double  value, Attack mainAttack) => value;
            setAttackStatChange = (double  value, Attack mainAttack) => value; 
            setSelfAttackStatChange = (double  value, Attack mainAttack) => value; 
            setMagicAttackStatChange = (double  value, Attack mainAttack) => value; 
            setSelfMagicAttackStatChange = (double  value, Attack mainAttack) => value; 
            setSpeedStatChange = (double  value, Attack mainAttack) => value;
            setSelfSpeedStatChange = (double  value, Attack mainAttack) => value; 
            setManaDrain = (double  value, Attack mainAttack) =>value;
            setSelfManaDrain = (double  value, Attack mainAttack) => value;

        }

        public AttackDecorator Clone()
        {
            AttackDecorator newDec = new AttackDecorator(null,null);
            newDec.setDamage = setDamage;
            newDec.setSelfHeal = setSelfHeal;
            newDec.setPoison = setPoison;
            newDec.setBurn = setBurn;
            newDec.setMentalBreak = setMentalBreak;
            newDec.setArmorChange = setArmorChange;
            newDec.setSelfArmorChange = setSelfArmorChange;
            newDec.setDefenseStatChange = setDefenseStatChange;
            newDec.setMagicResistStatChange = setMagicResistStatChange;
            newDec.setSelfMagicResistStatChange = setSelfMagicResistStatChange;
            newDec.setSelfDefenseStatChange = setSelfDefenseStatChange;
            newDec.setAttackStatChange = setAttackStatChange;
            newDec.setSelfAttackStatChange = setSelfAttackStatChange;
            newDec.setMagicAttackStatChange = setMagicAttackStatChange;
            newDec.setSelfMagicAttackStatChange = setSelfMagicAttackStatChange;
            newDec.setSpeedStatChange = setSpeedStatChange;
            newDec.setSelfSpeedStatChange = setSelfSpeedStatChange;
            newDec.setManaDrain = setManaDrain;
            newDec.setSelfManaDrain = SetSelfManaDrain;
            
            newDec.setCalculateDamage = setCalculateDamage;
            newDec.Type = Type;
            return newDec;
        }

        public override double CalculateDamage(double damage)
        {
            double baseValue = layerUpAttack.CalculateDamage(damage);
            if (setCalculateDamage != null)
            {
                return setCalculateDamage(baseValue,MainAttack);
            }
            return baseValue;
        }

        public override double Damage { get => setDamage(layerUpAttack.Damage, MainAttack); set => layerUpAttack.Damage = value; }
        public override double SelfHeal { get => setSelfHeal(layerUpAttack.SelfHeal, MainAttack); set => layerUpAttack.SelfHeal = value; }
        public override double Poison { get => setPoison(layerUpAttack.Poison, MainAttack); set => layerUpAttack.Poison = value; }

        public override double Burn { get => setBurn(layerUpAttack.Burn, MainAttack); set => layerUpAttack.Burn = value; }
        public override double MentalBreak { get => setMentalBreak(layerUpAttack.MentalBreak,MainAttack); set => layerUpAttack.MentalBreak = value; }
        public override double ArmorChange { get => setArmorChange(layerUpAttack.ArmorChange, MainAttack); set => layerUpAttack.ArmorChange = value; }
        public override double SelfArmorChange { get => setSelfArmorChange(layerUpAttack.SelfArmorChange, MainAttack); set => layerUpAttack.SelfArmorChange = value; }
        public override double DefenseStatChange { get => setDefenseStatChange(layerUpAttack.DefenseStatChange, MainAttack); set => layerUpAttack.DefenseStatChange = value; }
        public override double MagicResistStatChange { get => setMagicResistStatChange(layerUpAttack.MagicResistStatChange, MainAttack); set => layerUpAttack.MagicResistStatChange = value; }
        public override double SelfMagicResistStatChange { get => setSelfMagicResistStatChange(layerUpAttack.SelfMagicResistStatChange, MainAttack); set => layerUpAttack.SelfMagicResistStatChange = value; }
        public override double SelfDefenseStatChange { get => setSelfDefenseStatChange(layerUpAttack.SelfDefenseStatChange, MainAttack); set => layerUpAttack.SelfDefenseStatChange = value; }
        public override double AttackStatChange { get => setAttackStatChange(layerUpAttack.AttackStatChange, MainAttack); set => layerUpAttack.AttackStatChange = value; }
        public override double SelfAttackStatChange { get => setSelfAttackStatChange(layerUpAttack.SelfAttackStatChange, MainAttack); set => layerUpAttack.SelfAttackStatChange = value; }
        public override double MagicAttackStatChange { get => setMagicAttackStatChange(layerUpAttack.MagicAttackStatChange, MainAttack); set => layerUpAttack.MagicAttackStatChange = value; }
        public override double SelfMagicAttackStatChange { get => setSelfMagicAttackStatChange(layerUpAttack.SelfMagicAttackStatChange, MainAttack); set => layerUpAttack.SelfMagicAttackStatChange = value; }
        public override double SpeedStatChange { get => setSpeedStatChange(layerUpAttack.SpeedStatChange, MainAttack); set => layerUpAttack.SpeedStatChange = value; }
        public override double SelfSpeedStatChange { get => setSelfSpeedStatChange(layerUpAttack.SelfSpeedStatChange, MainAttack); set => layerUpAttack.SelfSpeedStatChange = value; }
        public SetStatsDelegate SetDamage { get => setDamage; set => setDamage = value; }
        public SetStatsDelegate SetSelfHeal { get => setSelfHeal; set => setSelfHeal = value; }
        public SetStatsDelegate SetPoison { get => setPoison; set => setPoison = value; }
        public SetStatsDelegate SetMentalBreak { get => setMentalBreak; set => setMentalBreak = value; }
        public SetStatsDelegate SetArmorChange { get => setArmorChange; set => setArmorChange = value; }
        public SetStatsDelegate SetSelfArmorChange { get => setSelfArmorChange; set => setSelfArmorChange = value; }
        public SetStatsDelegate SetDefenseStatChange { get => setDefenseStatChange; set => setDefenseStatChange = value; }
        public SetStatsDelegate SetMagicResistStatChange { get => setMagicResistStatChange; set => setMagicResistStatChange = value; }
        public SetStatsDelegate SetSelfMagicResistStatChange { get => setSelfMagicResistStatChange; set => setSelfMagicResistStatChange = value; }
        public SetStatsDelegate SetSelfDefenseStatChange { get => setSelfDefenseStatChange; set => setSelfDefenseStatChange = value; }
        public SetStatsDelegate SetAttackStatChange { get => setAttackStatChange; set => setAttackStatChange = value; }
        public SetStatsDelegate SetSelfAttackStatChange { get => setSelfAttackStatChange; set => setSelfAttackStatChange = value; }
        public SetStatsDelegate SetMagicAttackStatChange { get => setMagicAttackStatChange; set => setMagicAttackStatChange = value; }
        public SetStatsDelegate SetSelfMagicAttackStatChange { get => setSelfMagicAttackStatChange; set => setSelfMagicAttackStatChange = value; }
        public SetStatsDelegate SetSpeedStatChange { get => setSpeedStatChange; set => setSpeedStatChange = value; }
        public SetStatsDelegate SetSelfSpeedStatChange { get => setSelfSpeedStatChange; set => setSelfSpeedStatChange = value; }
        public SetStatsDelegate SetManaDrain { get => setManaDrain; set => setManaDrain = value; }
        public SetStatsDelegate SetSelfManaDrain { get => setSelfManaDrain; set => setSelfManaDrain = value; }
        public InflictDamageDelegate SetInflictDamage { get => setInflictDamage; set => setInflictDamage = value; }
        public InflictDamageDelegate SetCalculateDamage { get => setCalculateDamage; set => setCalculateDamage = value; }
        public Attack MainAttack { get => mainAttack; set => mainAttack = value; }
    }
}
