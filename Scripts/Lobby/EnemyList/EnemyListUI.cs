using _GAME.Scripts.Inventory;
using UnityEngine;

public class EnemyListUI : MonoBehaviour
{
    [SerializeField] private GameObject enemyCell;
    [SerializeField] private Transform content;

    void Start()
    {
        var enemySettings = GameManager.Instance.Resources.AllEnemySettings;

        foreach(SkinSettings enemy in enemySettings.Values)
        {
            var _enemyCell = Instantiate(enemyCell, Vector3.zero, Quaternion.identity, content);
            _enemyCell.GetComponent<EnemyCell>()?.SetData(enemy.skin);
        }
    }
}