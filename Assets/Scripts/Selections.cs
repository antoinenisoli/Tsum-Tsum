using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Selections : MonoBehaviour
{
    Camera mainCam => Camera.main;

    [Header("Feedbacks")]
    [SerializeField] LayerMask petLayer;

    [Header("Pets")]
    public RegularPet myPetReference;
    public List<Pet> selectedPets = new List<Pet>();
    [HideInInspector] public List<Pet> allPets;

    void SelectPets()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, petLayer);
            if (hit)
            {
                if (hit.transform.GetComponent<RegularPet>())
                {
                    if (myPetReference == null)
                        hit.transform.GetComponent<RegularPet>().Select();
                }
                else if (hit.transform.GetComponent<BonusPet>())
                {
                    hit.transform.GetComponent<BonusPet>().Effect();
                }

                if (selectedPets.Count > 1)
                {
                    for (int i = 1; i < selectedPets.Count; i++)
                        Debug.DrawLine(selectedPets[i].transform.position, selectedPets[i - 1].transform.position, Color.red);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
            DestroyTiles();
    }

    void DestroyTiles()
    {
        myPetReference = null;
        foreach (var pet in allPets)
            pet.Clear();

        if (selectedPets.Count >= 3)
        {
            bool correct = false;
            Physics2D.queriesStartInColliders = false;
            for (int i = 1; i < selectedPets.Count; i++)
            {
                RaycastHit2D hit = Physics2D.Linecast(selectedPets[i].transform.position, selectedPets[i - 1].transform.position, petLayer);
                correct = hit.transform == selectedPets[i - 1].transform;

                if (!correct)
                    break;
            }

            Physics2D.queriesStartInColliders = true;
            if (correct)
            {
                SendScore(selectedPets);
                float delay = 0;
                foreach (var pet in selectedPets)
                {
                    pet.Death(delay);
                    delay += 0.05f;
                    EventManager.Instance.onNewPet.Invoke(pet.destroyDuration + delay);
                }
            }
        }

        selectedPets.Clear();
    }

    public void SendScore(List<Pet> pets)
    {
        int score = 0;
        foreach (var pet in pets)
        {
            score += pet.scoreValue + (pet.scoreValue * pet.petId);
        }

        EventManager.Instance.onAddScore.Invoke(score);
    }

    private void Update()
    {
        SelectPets();
    }
}
