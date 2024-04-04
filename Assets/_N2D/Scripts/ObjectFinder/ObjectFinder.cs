using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StinkySteak.N2D.Finder
{
    public static class ObjectFinder
    {
        public static List<T> Find<T>(int interfaceListPreAlloc = 0, int rootGameObjectListPreAlloc = 0, int getComponentsListPreAlloc = 0)
        {
            List<T> interfaces = new List<T>(interfaceListPreAlloc);
            List<GameObject> rootGameObjects = new List<GameObject>(rootGameObjectListPreAlloc);
            List<T> getComponentInChildrens = new List<T>(getComponentsListPreAlloc);
            SceneManager.GetActiveScene().GetRootGameObjects(rootGameObjects);

            foreach (var rootGameObject in rootGameObjects)
            {
                getComponentInChildrens.Clear();
                rootGameObject.GetComponentsInChildren(getComponentInChildrens);
                foreach (var childInterface in getComponentInChildrens)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }

        private const int ROOT_OBJECT_CAPACITY = 200;
        private static List<GameObject> _rootGameObjectsPreAlloc = new(ROOT_OBJECT_CAPACITY);

        public static List<T> FindPreAlloc<T>()
        {
            _rootGameObjectsPreAlloc.Clear();

            int interfaceListPreAlloc = 10;
            int getComponentsListPreAlloc = 50;

            List<T> interfaces = new List<T>(interfaceListPreAlloc);
            List<T> getComponentInChildrens = new List<T>(getComponentsListPreAlloc);
            SceneManager.GetActiveScene().GetRootGameObjects(_rootGameObjectsPreAlloc);

            foreach (var rootGameObject in _rootGameObjectsPreAlloc)
            {
                getComponentInChildrens.Clear();
                rootGameObject.GetComponentsInChildren(getComponentInChildrens);
                foreach (var childInterface in getComponentInChildrens)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }

        public static List<T> FindPreAlloc<T>(Scene scene)
        {
            _rootGameObjectsPreAlloc.Clear();

            int interfaceListPreAlloc = 10;
            int getComponentsListPreAlloc = 50;

            List<T> interfaces = new List<T>(interfaceListPreAlloc);
            List<T> getComponentInChildrens = new List<T>(getComponentsListPreAlloc);
            scene.GetRootGameObjects(_rootGameObjectsPreAlloc);

            foreach (var rootGameObject in _rootGameObjectsPreAlloc)
            {
                getComponentInChildrens.Clear();
                rootGameObject.GetComponentsInChildren(getComponentInChildrens);
                foreach (var childInterface in getComponentInChildrens)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }

        public static List<T> FindFast<T>()
        {
            int interfaceListPreAlloc = 10;
            int rootGameObjectListPreAlloc = 100;
            int getComponentsListPreAlloc = 50;

            List<T> interfaces = new List<T>(interfaceListPreAlloc);
            List<GameObject> rootGameObjects = new List<GameObject>(rootGameObjectListPreAlloc);
            List<T> getComponentInChildrens = new List<T>(getComponentsListPreAlloc);
            SceneManager.GetActiveScene().GetRootGameObjects(rootGameObjects);

            foreach (var rootGameObject in rootGameObjects)
            {
                getComponentInChildrens.Clear();
                rootGameObject.GetComponentsInChildren(getComponentInChildrens);
                foreach (var childInterface in getComponentInChildrens)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }
    }
}