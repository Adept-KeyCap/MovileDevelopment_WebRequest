using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokemonHttp : MonoBehaviour
{
    
    private string MortyUrl = "https://pokeapi.co/api/v2";
    private string ImageLink = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/";
    [SerializeField] private RawImage[] characterQuads;
    [SerializeField] private TextMeshProUGUI[] cardNames;
    [SerializeField] private TextMeshProUGUI userText;
    [SerializeField] private TMP_InputField _inputField;
    private int userIndex=0;
    [SerializeField] private TextMeshProUGUI deckBtnText;
    private string currentUser = "";
    private int[] currentDeckCards=new int[4]; 
    private string fakeApiUrl = "https://my-json-server.typicode.com/juansecadavid/FakeAPI";


    public void SendRequest()
    {
        /*for (int i = 0; i < 4; i++)
        {
            StartCoroutine(GetCharacter(currentDeckCards[i],i));
        }*/
        StartCoroutine(GetCharacter(_inputField.text.ToLower(), currentDeckCards[0]));
    }

    IEnumerator GetCharacter(string image, int index)
    {
        Debug.Log("Llamaste");
        UnityWebRequest request = UnityWebRequest.Get(MortyUrl+$"/pokemon/{image}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                CharacterImage data = JsonUtility.FromJson<CharacterImage>(request.downloadHandler.text);
                Debug.Log(data.id);
                StartCoroutine(DownloadImage(ImageLink+data.id+".png",index));
                //cardNames[index].text = data.name;
                cardNames[index].text = $"Su altura es {data.height} \n Y su peso es {data.weight}";
            }
            else
            {
                Debug.Log($"{request.responseCode}|{request.error}");
            }
        }
    }

    IEnumerator DownloadImage(string url, int pos)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            characterQuads[pos].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            characterQuads[pos].gameObject.SetActive(true);
        }
    }
    
}
[System.Serializable]
public class JsonData
{
    public CharacterImage[] users;
}
[System.Serializable]
public class CharacterImage
{
    //public CardsDeck[] sprites;
    public string name;
    public Types types;
    public string id;
    public string weight;

    public string height;
    //public CardsDeck[] deck;
}

[System.Serializable]
public class Types
{
    public PokeType type;
    public string slot;
}
[System.Serializable]
public class PokeType
{
    public string name;
    public string url;
}
