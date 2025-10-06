using QFramework;
using UnityEngine;
using System.Collections.Generic;

namespace SkateGame
{//For scoring system of tricks
    public interface ITrickSystem : ISystem
    {
        void AddTrick(TrickState trick);
        void RemoveAllTricks();
        string GetTrickName(TrickState trick);
        int GetTrickScore(TrickState trick);
        int SumOfScore();
        char DetectGrade(int sumScore);
        BindableProperty<List<TrickState>> TrickList { get; }
        BindableProperty<char> Grade { get; }
    }

    public class TrickSystem : AbstractSystem, ITrickSystem, ICanSendCommand
    {
        private ITrickListModel trickListModel;
        
        // 暴露TrickListModel的属性
        public BindableProperty<List<TrickState>> TrickList => trickListModel.TrickList;
        public BindableProperty<char> Grade => trickListModel.Grade;
        
        protected override void OnInit()
        {
            // 获取TrickListModel
            trickListModel = this.GetModel<ITrickListModel>();
        }

        #region Public Methods
        
        public void AddTrick(TrickState trick)
        {
            if (trick != null)
            {
                trickListModel.TrickList.Value.Add(trick);
                
            }
        }

        public void RemoveAllTricks()
        {
            trickListModel.TrickList.Value.Clear();
           
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
            foreach (TrickState trick in trickListModel.TrickList.Value)
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
        
        #endregion

        #region Private Methods
        
        private void UpdateGrade()
        {
            int totalScore = SumOfScore();
            trickListModel.Grade.Value = DetectGrade(totalScore);
            Debug.Log($"TrickSystem: 更新等级 - 总分: {totalScore}, 等级: {trickListModel.Grade.Value}");
        }
        
        #endregion
    }
}
