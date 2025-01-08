using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;

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
            public void set_data(int val) {
                data = val;
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
        private IHashFunction hash_func;
        private int l;

        public hash_table(IHashFunction function, int int_l) {
            hash_func = function;
            l = int_l;
            long size = (long)Math.Pow(2, int_l);
            this.hashTable = new hash_node[size];

            for (int i = 0; i < size; i++) {
                this.hashTable[i] = null;
            }
        }
        
        public void insert(Tuple<ulong, int> keys) {
            hash_node new_node = new hash_node(keys);
            ulong hash = hash_func.hash_function(keys.Item1);
            hash_node prev = hashTable[hash];
            hash_node current = hashTable[hash];

            if (hashTable[hash] == null) {
                hashTable[hash] = new_node;
                return;
            }
            
            while (current != null && current.get_key() != keys.Item1) {
                prev = current;
                current = current.get_nextNode();   
            }
            if (current != null && current.get_key() == keys.Item1) {
                this.increment(current.get_key(), keys.Item2);
                return;
                }
            new_node.set_nextNode(prev.get_nextNode());
            prev.set_nextNode(new_node);
        }

        public void increment(ulong key, int increment_val) {
            ulong hash = hash_func.hash_function(key);
            hash_node current = hashTable[hash];
            if (current is null) {
                this.insert(Tuple.Create(key, increment_val));
                return;
            }
            while (current != null) {
                if (current.get_key() == key) {
                    current.set_data(current.get_data() + increment_val);
                    return;
                }
                current = current.get_nextNode();
            }
            this.insert(Tuple.Create(key, increment_val));
        }

        public int get(ulong key) {
            ulong hash = hash_func.hash_function(key);
            
            hash_node current = hashTable[hash];

            if (hashTable[hash] == null) {
                return 0;
            }
            while (current != null) {
                if (current.get_key() == key) {
                    return current.get_data();
                }
                current = current.get_nextNode();
            }

            return 0;
        }

        public void set(ulong key, int val) {
            ulong hash = hash_func.hash_function(key);
            hash_node current = hashTable[hash];
            if (hashTable[hash] is null) {
                this.insert(Tuple.Create(key, val));
                return;
            }
            while (current != null) {
                if (current.get_key() == key) {
                    current.set_data(val);
                    return;
                }
                current = current.get_nextNode();
            }
            this.insert(Tuple.Create(key, val));
        }
    }
}