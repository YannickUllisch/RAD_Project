using System;
using System.Collections.Generic;
using System.Numerics;

namespace RAD_Project {

    public class HashNode {
            private ulong key;
            private HashNode next;
            public HashNode(ulong key) {
                this.key = key;
                next = null;
            }
            public ulong getKey() {
                return key;
            }
            public void setNextNode(HashNode node) {
                next = node;
            }
            public HashNode getNextNode() {
                return this.next;
            }
        }
    public class HashTable {
        public HashNode[] hashTable;

        public void init_table(int size) {
            this.hashTable = new HashNode[size];

            for (int i = 0; i < size; i++) {
                this.hashTable[i] = null;
            }
        }
    }
}