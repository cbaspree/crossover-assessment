using System;
using UnityEngine;

[Serializable]
public class BlockModel
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string subject;
    [SerializeField]
    private string grade;
    [SerializeField]
    private int mastery;
    [SerializeField]
    private string domainid;
    [SerializeField]
    private string domain;
    [SerializeField]
    private string cluster;
    [SerializeField]
    private string standardid;
    [SerializeField]
    private string standarddescription;

    public string Grade { get => grade; }
}