using QFramework;
using System.Collections.Generic;
using UnityEngine;

namespace SkateGame
{
    public interface ITrickListModel : IModel
    {
        BindableProperty<List<TrickState>> TrickList { get; }
        BindableProperty<char> Grade { get; }
        void AddTrick(TrickState trick);
        void RemoveAllTricks();
        string GetTrickName(TrickState trick);
        int GetTrickScore(TrickState trick);
        int SumOfScore();
        char DetectGrade(int sumScore);
    }

    public class TrickListModel : AbstractModel, ITrickListModel
    {
        public BindableProperty<List<TrickState>> TrickList { get; } = new BindableProperty<List<TrickState>>(new List<TrickState>());
        public BindableProperty<char> Grade { get; } = new BindableProperty<char>('D');

        protected override void OnInit()
        {
            // 初始化逻辑
        }

        public void AddTrick(TrickState trick)
        {
            if (trick != null)
            {
                TrickList.Value.Add(trick);
                UpdateGrade();
            }
        }

        public void RemoveAllTricks()
        {
            TrickList.Value.Clear();
            UpdateGrade();
        }

        public string GetTrickName(TrickState trick)
        {
            return trick?.TrickName ?? "";
        }

        public int GetTrickScore(TrickState trick)
        {
            return trick?.ScoreValue ?? 0;
        }

        public int SumOfScore()
        {
            int sum = 0;
            foreach (TrickState trick in TrickList.Value)
            {
                sum += GetTrickScore(trick);
            }
            return sum;
        }

        public char DetectGrade(int sumScore)
        {
            switch (sumScore)
            {
                case 10:
                    return 'C';
                case 20:
                    return 'B';
                case 30:
                    return 'A';
                default:
                    return 'D';
            }
        }

        private void UpdateGrade()
        {
            int totalScore = SumOfScore();
            Grade.Value = DetectGrade(totalScore);
        }
    }

}