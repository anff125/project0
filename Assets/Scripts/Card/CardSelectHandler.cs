using System.Collections;
using Card;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//for every card in the game, this script will be attached to it

public class CardSelectHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler,
    IDeselectHandler, IPointerClickHandler
{
    private float _verticalMoveAmount = 100f;
    private float _moveTime = 0.1f;
    private float _scaleAmount = 1.1f;

    private Vector3 _startPos;
    private Vector3 _startScale;

    private void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }

    private IEnumerator MoveCard(bool startAnimation)
    {
        Vector3 _endScale;
        Vector3 _endPos;

        float elapsedTime = 0;
        while (elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;
            if (startAnimation)
            {
                _endPos = _startPos + new Vector3(0f, _verticalMoveAmount);
                _endScale = _startScale * _scaleAmount;
            }
            else
            {
                _endPos = _startPos;
                _endScale = _startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, _endPos, elapsedTime / _moveTime);
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, _endScale, elapsedTime / _moveTime);

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.position = _startPos + new Vector3(0f, _verticalMoveAmount);
        transform.localScale = _startScale * _scaleAmount;
        CardSelectManager.Instance.LastSelectedCard = gameObject;

        //find the index
        for (int i = 0; i < CardSelectManager.Instance.Cards.Length; i++)
        {
            if (CardSelectManager.Instance.Cards[i] == gameObject)
            {
                CardSelectManager.Instance.LastSelectedCardIndex = i;
                break;
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //StartCoroutine(MoveCard(false));
        transform.position = _startPos;
        transform.localScale = _startScale;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}