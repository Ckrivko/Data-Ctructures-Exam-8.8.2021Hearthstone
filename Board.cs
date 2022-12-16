using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Board : IBoard
{
    private Dictionary<string, Card> cards;

    public Board()
    {
        this.cards = new Dictionary<string, Card>();
    }

    public bool Contains(string name)
    {
        return this.cards.ContainsKey(name);
    }

    public int Count()
    {
        return this.cards.Count;
    }

    public void Draw(Card card)
    {

        if (this.cards.ContainsKey(card.Name)) throw new ArgumentException();

        this.cards.Add(card.Name, card);
    }

    public IEnumerable<Card> GetBestInRange(int start, int end)
    {

        return this.cards.Values.Where(x => x.Score >= start && x.Score <= end)
            .OrderByDescending(x => x.Level);

    }

    public void Heal(int health)
    {
        this.cards.Values.OrderBy(x => x.Health).First().Health += health;

    }

    public IEnumerable<Card> ListCardsByPrefix(string prefix)
    {
        return this.cards.Values.Where(x => x.Name.StartsWith(prefix))
            .OrderBy(x => (int)x.Name[x.Name.Length - 1])
            .ThenBy(x => x.Level);
    }

    public void Play(string attackerCardName, string attackedCardName)
    {

        if (!this.cards.ContainsKey(attackerCardName)
            || !this.cards.ContainsKey(attackedCardName)) throw new ArgumentException();

        if (cards[attackerCardName].Level != cards[attackedCardName].Level) throw new ArgumentException();

        var attacker = cards[attackerCardName];
        var defender = cards[attackedCardName];

        if (!defender.IsDead && !attacker.IsDead)
        {
            defender.Health -= attacker.Damage;

            if (defender.IsDead)
            {
                attacker.Score += defender.Level;
            }

        }

    }

    public void Remove(string name)
    {
        if (!this.cards.ContainsKey(name)) throw new ArgumentException();

        this.cards.Remove(name);
    }

    public void RemoveDeath()
    {
        this.cards = this.cards.Where(x => x.Value.IsDead == false).ToDictionary(t => t.Key, t => t.Value);
    }

    public IEnumerable<Card> SearchByLevel(int level)
    {
        return this.cards.Values.Where(x => x.Level == level).OrderByDescending(x => x.Score);
    }
}
