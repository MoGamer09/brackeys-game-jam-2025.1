using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndScreenManager : MonoBehaviour
{
    private SituationGenerator _situationGenerator;
    public GameObject orderPagePrefab;
    public Vector2 ordersStartPos;
    public Vector2 ordersEndPos;
    public GameObject completedStampPrefab;
    public GameObject crashedStampPrefab;
    public Vector3 orderScaleInLineUp;
    public Vector2 orderShowPosition;
    public Vector2 stampPosition;
    public Vector2 orderSpawnPos;
    public Sprite filledStarSprite;
    public Sprite unFilledStarSprite;
    public List<SpriteRenderer> stars;
    
    private ScreenTinter _screenTinter;
    

    private void Awake()
    {
        _situationGenerator = GetComponent<SituationGenerator>();
        _screenTinter = GetComponentInChildren<ScreenTinter>();
    }

    public void ShowEndScreen()
    {
        _screenTinter.SetScreenTint(true);
        var seq = LeanTween.sequence();

        var situationCount = _situationGenerator.situations.Length;

        var completedOrders = 0;
        for (var situationIndex = 0; situationIndex < situationCount; situationIndex++)
        {
            var situation = _situationGenerator.situations[situationIndex];
            var situationCompleted = situation.car.GetComponentInChildren<CarController>().GetPathSize() ==
                                     situation.car.GetComponentInChildren<CarController>().GetPath()
                                         .Length; //This is not given because path size is the maximum simulated length
            
            if(situationCompleted)
                completedOrders++;
            
            var orderPage = Instantiate(orderPagePrefab, transform);
            orderPage.transform.position = orderSpawnPos;
            
            orderPage.GetComponent<OrderTextPopulator>().UpdateOrderText(situation.order);
            
            var stamp = Instantiate(situationCompleted ? completedStampPrefab : crashedStampPrefab, orderPage.transform);

            var children = orderPage.GetComponentsInChildren<SpriteRenderer>();
            for (var i = 0; i < children.Length; i++)
            {
                var spriteRenderer = children[i];
                spriteRenderer.sortingOrder = 601 + situationIndex * 2;
            }

            orderPage.GetComponentInChildren<Canvas>().sortingOrder = 601 + situationIndex * 2;
            
            stamp.GetComponent<SpriteRenderer>().sortingOrder = 601 + situationIndex * 2;
            
            orderPage.GetComponent<SpriteRenderer>().sortingOrder = 600 + situationIndex * 2;
            
            LeanTween.scale(stamp, Vector3.one * 3, 0f);
            stamp.GetComponent<SpriteRenderer>().color = Color.clear;
            stamp.transform.localPosition = stampPosition + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            stamp.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-20, 20));
            
            seq.append(LeanTween.moveLocal(orderPage, orderShowPosition, 0.3f).setEase(LeanTweenType.easeOutExpo));
            seq.append(() =>
            {
                LeanTween.scale(stamp, Vector3.one * 1.5f, 0.4f).setEase(LeanTweenType.easeOutExpo);
                LeanTween.color(stamp, Color.white, 0.1f);
            });
            seq.append(0.6f);
            var index = situationIndex;
            seq.append(() =>
            {
                LeanTween
                    .moveLocal(orderPage,
                        Vector2.Lerp(ordersStartPos, ordersEndPos, index / (situationCount - 1f)),
                        0.4f).setEase(LeanTweenType.easeInOutCubic);
                LeanTween.scale(orderPage, orderScaleInLineUp, 0.3f).setEase(LeanTweenType.easeInOutCubic);
            });
            seq.append(0.4f);
        }

        var score = completedOrders / (float)situationCount; //Score is 70 percent here

        for (int starIndex = 0; starIndex < stars.Count; starIndex++)
        {
            var star = stars[starIndex];
            var starPrecentage = ((1f / stars.Count) * (starIndex + 1)) - 0.01f;
            if (score >= starPrecentage)
            {
                var starScale = star.transform.localScale;
                star.sprite = filledStarSprite;
                seq.append(() => { star.enabled = true; });
                seq.append(LeanTween.scale(star.gameObject, starScale * 1.2f, 0.2f).setEase(LeanTweenType.easeInOutCubic));
                seq.append(LeanTween.scale(star.gameObject, starScale, 0.3f).setEase(LeanTweenType.easeInOutCubic));
            }
            else
            {
                star.sprite = unFilledStarSprite;
                star.enabled = true;
                star.color = Color.clear;
                seq.append(LeanTween.color(star.gameObject, Color.white, 0.5f).setEase(LeanTweenType.easeInOutCubic));
            }
        }
    }
}