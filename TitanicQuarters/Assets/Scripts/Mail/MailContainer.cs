using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MailContainer : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject _mailPrefab;

    [SerializeField]
    int _mailDisplayLimit = 5;

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
#if (UNITY_EDITOR) 

            if (!EditorApplication.isPlaying)
            {
                DestroyImmediate(mail);
                break;
            }
#endif
            Destroy(mail);
        }

        for (int i = 0; i < n; i++)
        {
            GameObject mail = Instantiate(_mailPrefab, GetPosition(i), transform.rotation * Quaternion.Euler(-35f, 180f, 0));
            mail.transform.SetParent(transform);
            _mailQueue.Add(mail);
        }
    }

#if (UNITY_EDITOR) 

    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            SetNumberOfMails(_CurrentNbrMail);
        }
    }

#endif

    Vector3 GetPosition(int i)
    {
        Vector3 step = (_queueEnd.position - _queueStart.position) / _mailDisplayLimit;
        return _queueStart.position + step * i;
    }




}
