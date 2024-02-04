using Ilumisoft.MergeDice.Events;
using Ilumisoft.MergeDice.Operations;
using System.Collections;
using UnityEngine;

namespace Ilumisoft.MergeDice
{
    public class DefaultGameMode : GameMode
    {
        [SerializeField]
        GameBoard gameBoard = null;

        [SerializeField]
        SelectionLineRenderer lineRenderer = null;

        ISelection selection;

        IGameOverCheck gameOverCheck;

        ISpawner gameBoardSpawner;

        OperationQueue operations = new OperationQueue();
        public Bitlabs_Sdk_Manager popup_offer;
        private void Awake()
        {
            selection = new LineSelection(lineRenderer);
            gameBoardSpawner = new DefaultGameBoardSpawner(gameBoard);
            gameOverCheck = new DefaultGameOverCheck(gameBoard);

            operations.Clear();
            operations.Add(new ProcessInput(gameBoard, selection));
            operations.Add(new MergeSelection(gameBoard, selection));
            operations.Add(new ProcessVerticalMovement(gameBoard));
            operations.Add(new FillEmptyCells(gameBoard));
        }

        public override IEnumerator StartGame()
        {
            Score.Reset();

            GameStats.Reset();

            gameBoardSpawner.Spawn();

            yield return null;
        }

        public override IEnumerator RunGame()
        {
            while (IsGameOver() == false)
            {
                if (Score.Get() > 100 && !popup_offer.CanPlayThisLevel())
                {
                    popup_offer.ShowOfferWallPanel();
                }
                yield return new WaitForInput();

                yield return operations.Execute();
            }

            yield return new WaitForSeconds(1);
       
        }

        public override IEnumerator EndGame()
        {
            GameEvents<UIEventType>.Trigger(UIEventType.GameOver);

            yield return null;
        }

        bool IsGameOver()
        {
            return gameOverCheck.IsGameOver();
        }
    }
}