function OnTriggerEnter (other : Collider) { if(other.gameObject.tag == "Player")

GetComponent.<Renderer>().material.mainTexture.Pause();
Application.LoadLevel("Menu");

}