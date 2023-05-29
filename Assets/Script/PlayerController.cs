using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float mouseSensitivityX = 10f;
    [SerializeField]
    private float mouseSensitivityY = 8f;

    [SerializeField]
    private float thrusterforce = 1000f;
    [Header("Joint Options")]
    [SerializeField]
    private float jointSpring = 4;
    [SerializeField]
    private float jointMaxForce = 30;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start() {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        StetJointSettingd(jointSpring);
    }

    private void Update() {

        //Player move
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");
        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

        //player rotate
        float yRoat = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRoat, 0) * mouseSensitivityX;

        motor.Rotate(rotation);

        //camera rotate
        float xRoat = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRoat * mouseSensitivityY;

        motor.RotateCamera(cameraRotationX);

        //jetpark / thruster
        Vector3 thrusterVelocity = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            StetJointSettingd(0f);
            thrusterVelocity = Vector3.up  * thrusterforce;
        }else{
            StetJointSettingd(jointSpring);
        }

        motor.ApplyThruster(thrusterVelocity);
    }

    private void StetJointSettingd(float _jointSpring){
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce};
    }
}