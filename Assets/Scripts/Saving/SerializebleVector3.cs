using System;

namespace RPG.Saving {
    [Serializable]
    class SerializebleVector3 {
        public float x, y, z;

        public SerializebleVector3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}