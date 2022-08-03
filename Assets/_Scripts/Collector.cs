using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [Header("Ref")]
    public Transform ItemHolderTransform;
    [SerializeField] Transform BoxArea;

    int NumOfItemsHolding = 0;

    public void AddNewItem(Transform _itemToAdd)
    {

        _itemToAdd.DOJump(ItemHolderTransform.position, 7f, 1, .40f).OnComplete(() =>
            {
                _itemToAdd.SetParent(ItemHolderTransform, true);//Set the parent of the transform.
                _itemToAdd.localPosition = new Vector3(0, NumOfItemsHolding+_itemToAdd.transform.localScale.y, 0);
                _itemToAdd.localRotation = Quaternion.identity;
                NumOfItemsHolding++;

            }
        );

    }

    public void DropItem(Transform _itemToAdd)
    {
        //print(_itemToAdd);
        _itemToAdd.DOJump(BoxArea.position, 7f, 1, .40f).OnComplete(() =>
            {
                _itemToAdd.parent = null;
                //_itemToAdd.localPosition = BoxArea.position;
                //_itemToAdd.GetComponent<BoxCollider>().isTrigger = false;
                _itemToAdd.localRotation = Quaternion.identity;
                NumOfItemsHolding--;
                Destroy(_itemToAdd.gameObject);
                GameManager.instance.MoneyEarned();
                GameManager.instance.CheckUpgradeButtons();

            }
        );

    }
}
