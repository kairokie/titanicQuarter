using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailContainer : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private WordMachine _wordMachine;

    [SerializeField]
    private GameObject _mailPrefab;

    [SerializeField] 
    float _mailDisplayLimit = 5;

    [SerializeField]
    Transform _queueStart;

    [SerializeField]
    Transform _queueEnd;

    private List<GameObject> _mailQueue = new List<GameObject>();

    public int _CurrentNbrMail = 0;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNumberOfMails(int n)
    {
        n = Mathf.Clamp(n, 0, (int)_mailDisplayLimit);
        foreach (var mail in _mailQueue)
        {
            Destroy(mail);
            //DestroyImmediate(mail);
        }

        for (int i = 0; i < n; i++)
        {
            GameObject mail = Instantiate(_mailPrefab, GetPosition(i), Quaternion.Euler(-65,0,0));
            mail.transform.SetParent(transform);
            _mailQueue.Add(mail);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    SetNumberOfMails(_CurrentNbrMail);
    //}

    Vector3 GetPosition(int i)
    {
        Vector3 step = (_queueEnd.position - _queueStart.position) / _mailDisplayLimit;
        return _queueStart.position + step * i;
    }

    


}
