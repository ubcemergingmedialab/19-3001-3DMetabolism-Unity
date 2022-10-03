using System.Collections;
using System.Collections.Generic;
using SimpleJSON;                   // I think we can use internal JSON capabilities now, but I used this as I'm familiar with it.
using UnityEngine;
using UnityEngine.Networking;

public class CompoundController : MonoBehaviour
{
    public int CID = 241;        // Pubchem Compound ID  (241 = Benzene, our test atom)
    private string jsonURL;         // The basic URL for Pubchem compounds

    // Start is called before the first frame update
    void Start()
    {
//        StartCoroutine(loadCompound(241));
    }
    void OnEnable()
    {
        StartCoroutine(loadCompound(10267));
    }

    // From https://answers.unity.com/questions/21174/create-cylinder-primitive-between-2-endpoints.html, translated hastily from UnityScript
    /// <summary>
    /// This takes a cylinder (or any other prefab that uses the Y axis to connect between locatoins), and stretches it between a start and end Vector3, at a given width.
    /// </summary>
    /// <param name="bondType">This indicates which prefab to use. 
    /// These must be available under Resources/Prefabs/ and should have the form BondX where X is the bondType. 
    /// If no object is found, the bondMissing prefab is used. Modifying this generalizes the function beyond bonds.</param>
    /// <param name="start">The first Vector3 to connect</param>
    /// <param name="end">The second Vector3 to connect</param>
    /// <param name="width">The width of the prefab to use</param>
    private void CreateBond(int bondType, Vector3 start, Vector3 end, float width)
    {
        var offset = end - start;
        var scale = new Vector3(width, (float)offset.magnitude / 2f, width);
        //var position = start + (offset / 2.0);
        var position = new Vector3(start.x + (offset.x / 2), start.y + (offset.y / 2), start.z + (offset.z / 2));

        GameObject bondObj = (GameObject)Resources.Load("Prefabs/bond" + bondType);

        // If we don't have a bondObj from Resources, use bondMissing instead. This is red by default, to indicate it's a missing element. Maybe use Unity pink instead?
        if (bondObj == null)
        {
            Debug.Log("Can't Find Prefabs/bond" + bondType);
            bondObj = (GameObject)Resources.Load("Prefabs/bondMissing");
        }

        bondObj.layer = 5; // UI Layer
        GameObject bond = (GameObject)Instantiate(bondObj, position, Quaternion.identity);
        bond.transform.SetParent(transform, false);
        bond.transform.up = transform.localToWorldMatrix * offset;
        bond.transform.localScale = scale;
    }

    // Follow Object.Instantiate() format as closely as possible.
    public IEnumerator loadCompound(int compoundCID, Vector3 compoundPosition = default(Vector3), Quaternion compoundRotation = default(Quaternion), Transform compoundParent = null) {
        //if there are any, delete children before adding another compound
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        string jsonURL = "https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/CID/" + compoundCID + "/record/JSON/?record_type=3d&response_type=display";
        string compoundJSON = ""; 
        // Get JSON
        using (UnityWebRequest webRequest = UnityWebRequest.Get(jsonURL))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = jsonURL.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                compoundJSON = webRequest.downloadHandler.text;
            }
        }
        // Parse JSON
        var compound = JSON.Parse(compoundJSON);

        // Instantiate Atoms
        for (int i=0; i< compound["PC_Compounds"][0]["atoms"]["aid"].Count; i++)
        {
            float atomX = compoundPosition.x + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["x"][i];
            float atomY = compoundPosition.y + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["y"][i];
            float atomZ = compoundPosition.z + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["z"][i];
            int atomElement = compound["PC_Compounds"][0]["atoms"]["element"][i];

            GameObject atomObj = (GameObject)Resources.Load("Prefabs/atom" + atomElement);
            if (atomObj == null)
            {
                atomObj = (GameObject)Resources.Load("Prefabs/atomMissing");
                Debug.Log("Can't Find Prefabs/atom" + atomElement);
            }
            atomObj.layer = 5; //UI Layer
            GameObject instantiated = Instantiate(atomObj, new Vector3(atomX, atomY, atomZ), Quaternion.identity);
            instantiated.transform.SetParent(transform, false);
        }

        // Create Bonds
        for (int i = 0; i < compound["PC_Compounds"][0]["bonds"]["aid1"].Count; i++)
        {
            // Get the conformer coordinates for the 2 atoms whose indices are specified by ["bonds"]["aid1"] and ["bonds"]["aid2"]
            float atom1X = compoundPosition.x + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["x"][(int)compound["PC_Compounds"][0]["bonds"]["aid1"][i]-1];
            float atom1Y = compoundPosition.y + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["y"][(int)compound["PC_Compounds"][0]["bonds"]["aid1"][i]-1];
            float atom1Z = compoundPosition.z + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["z"][(int)compound["PC_Compounds"][0]["bonds"]["aid1"][i]-1];

            float atom2X = compoundPosition.x + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["x"][(int)compound["PC_Compounds"][0]["bonds"]["aid2"][i]-1];
            float atom2Y = compoundPosition.y + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["y"][(int)compound["PC_Compounds"][0]["bonds"]["aid2"][i]-1];
            float atom2Z = compoundPosition.z + compound["PC_Compounds"][0]["coords"][0]["conformers"][0]["z"][(int)compound["PC_Compounds"][0]["bonds"]["aid2"][i]-1];

            int bondType = compound["PC_Compounds"][0]["bonds"]["order"][i];

            CreateBond(bondType, new Vector3(atom1X, atom1Y, atom1Z), new Vector3(atom2X, atom2Y, atom2Z), 0.1f);
        }
    }
}