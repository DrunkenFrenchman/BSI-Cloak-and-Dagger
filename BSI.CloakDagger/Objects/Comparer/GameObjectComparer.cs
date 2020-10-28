using System.Collections.Generic;

namespace BSI.CloakDagger.Objects.Comparer
{
    public class GameObjectComparer : IEqualityComparer<GameObject>
    {
        public bool Equals(GameObject x, GameObject y)
        {
            return x != null && y != null && x.GameObjectType == y.GameObjectType && x.StringId == y.StringId;
        }

        public int GetHashCode(GameObject gameObject)
        {
            unchecked
            {
                return (gameObject.StringId.GetHashCode() * 397) ^ gameObject.StringId.GetHashCode();
            }
        }
    }
}