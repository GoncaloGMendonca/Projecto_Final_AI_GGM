using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    

    [SerializeField] Camera cam;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Text loadingBulletTxt;
    [SerializeField] Text amountbulletsText;
    [SerializeField] GameObject WinMenu;
    [SerializeField] int amountBullets = 10;
    [SerializeField] int maxBullets = 10;
    [SerializeField] float fireRate = 1f;
    [SerializeField] GameObject bulletObj;
    [SerializeField] GameObject GunShooter;
    [SerializeField] Camera PlayerCamera;
    [SerializeField] LayerMask GroundMask;
    bool aiming = false;

    
    [SerializeField] bool isShooting = false;
    public Vector3 mouseTarget;     
    float fireRateRef;
    bool canShoot = true;
    bool isLoading = false;
    [SerializeField] float loadtime = 5f;
    [SerializeField] float loadtimeref ;

    private void Start()
    {
        WinMenu.gameObject.SetActive(false);
        fireRate = 1f;
        isShooting = false;
        canShoot = true;
        fireRateRef = fireRate;
        loadtimeref = loadtime; 
        amountbulletsText.text = amountBullets.ToString();
    }

   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
           
            RaycastHit hit;
          
            if (Physics.Raycast(ray, out hit))
            {
               
                agent.SetDestination(hit.point);
            }  
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (canShoot)
            {
                ShootingBullet();
            }
        }
    

        if (aiming)
        {
            Vector3 directionToAim = UpdateMouseRayPostion(Input.mousePosition);
            directionToAim = directionToAim - transform.position;
           
            directionToAim.y = 0f;
            transform.forward = directionToAim;
        }

        if (isShooting)
        {
            if (fireRateRef <= 0)
            {
                fireRateRef = fireRate;
                isShooting = false;
                canShoot = true;
            }
            fireRateRef = fireRateRef - 1 * Time.deltaTime;
        }

        if (amountBullets <=0)
        {
           
            loadingBulletTxt.text = "Yes";
            isLoading = true;
            canShoot = false;
        }
        else
        {
            loadingBulletTxt.text = "No";

        }
        if (isLoading)
        {
            if (loadtimeref <=0)
            {
                loadtimeref = loadtime;
                isLoading = false;
                canShoot = true;
                amountBullets = 10;
                amountbulletsText.text = amountBullets.ToString();
            }
            loadtimeref = loadtimeref - 1 * Time.deltaTime;
        }

    }

    Vector3 UpdateMousePosition()
    {
        mouseTarget = Input.mousePosition;
      

       
        mouseTarget.y = 0f;
        var worldPos = Camera.main.ScreenToWorldPoint(mouseTarget);
        return worldPos;
    }

    private void FixedUpdate()
    {
        UpdateMouseRayPostion(Input.mousePosition);
    }


    Vector3 UpdateMouseRayPostion(Vector3 mousePosition)
    {
        var ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, GroundMask))
        {
            aiming = true;
            return hitInfo.point;
        }
        else
        {
            aiming = false;
            return Vector3.zero;
        }
    }

    void ShootingBullet()
    {
        if (!isShooting && amountBullets > 0)
        {
            
            GameObject obj = Instantiate(bulletObj, GunShooter.transform.position, Quaternion.identity, transform.parent);
            obj.GetComponent<Transform>().rotation = GunShooter.transform.localRotation;
            obj.GetComponent<Bullet>().StartBullet(GunShooter.transform.position);
            obj.GetComponent<Bullet>().StartBullet(this.transform.forward);
            isShooting = true;
            canShoot = false;
            amountBullets--;
            amountbulletsText.text = amountBullets.ToString();
        }
       

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WinGate")
        {
            Time.timeScale = 0f;
            WinMenu.gameObject.SetActive(true);
            Debug.Log("WonRound");
            Destroy(this.gameObject);
           
        }
      
    }

 

}
