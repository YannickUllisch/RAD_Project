using System;
using System.Collections.Generic;
using System.Numerics;

namespace RAD_Project {
    public class hash_node {
            private ulong key;
            private int data;
            private hash_node next;
            public hash_node(Tuple<ulong, int> keys) {
                this.key = keys.Item1;
                this.data = keys.Item2;
                next = null;
            }

            public int get_data() {
                return data;
            }
            public ulong get_key() {
                return key;
            }
            public void set_nextNode(hash_node node) {
                next = node;
            }
            public hash_node get_nextNode() {
                return this.next;
            }
        }
        
    public class hash_table {
        public hash_node[] hashTable;

        public void init_table(int size) {
            this.hashTable = new hash_node[size];

            for (int i = 0; i < size; i++) {
                this.hashTable[i] = null;
            }
        }

        public void insert(Tuple<ulong, int> keys, hash_functions function) {
            hash_node node = new hash_node(keys);
            ulong hash = function.mult_shift_hash(keys.Item1);

            if (hashTable[hash] != null && hashTable[hash].get_key() != keys.Item1) {
                
            } else if (hashTable[hash] != null && hashTable[hash].get_key() == keys.Item1) {
                
            }
        }
    }
}