namespace Control.Raycasts
{
    public struct CollisionInfo
    {
        public bool Above, Below;
        public int FaceDir;

        public void Reset()
        {
            Above = Below = false;
        }
    }
}