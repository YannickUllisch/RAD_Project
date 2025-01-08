# Randomized Algorithms Course Project: Hashing and Count-Sketch

This project involves implementing two different algorithms to analyze large streams of data. The objective is to compare their space and time efficiency when calculating a specific mathematical property of data streams. Below is a detailed explanation of the project requirements and structure.

---

## Project Overview

We were tasked with processing a stream of data represented as pairs `(x, d)`:
- `x` is a 64-bit key from a universe \( U = [2^{64}] \).
- `d` is an integer which can be positive or negative.

For each unique key `x`, we define \( s(x) \) as the weighted sum of `d` values associated with `x` in the stream. The goal is to compute the quadratic sum \( S = \sum_{x \in U} s(x)^2 \), which measures the variance of the data stream.

The project was divided into two parts:
1. **Exact Calculation Using Hashing with Chaining**
2. **Approximate Calculation Using Count-Sketch**

---

## Part 1: Hashing with Chaining

### Objective
Implement a hash table with chaining to calculate the quadratic sum \( S \) exactly.

### Key Steps
1. **Hash Function Implementation**
   - **Multiply-Shift Hashing:** \( h(x) = (a \cdot x) >> (64 - l) \), where \( a \) is a random 64-bit odd number.
   - **Multiply-Mod-Prime Hashing:** \( h(x) = ((a \cdot x + b) \mod p) \mod 2^l \), where \( p = 2^{89} - 1 \).

2. **Hash Table Implementation**
   - Operations include `get(x)`, `set(x, v)`, and `increment(x, d)`.
   - Used to store the weighted sum \( s(x) \) for each unique key \( x \).

3. **Performance Analysis**
   - Test run times for different hash functions and table sizes.
   - Compare the impact of hash function choice and the number of unique keys.

---

## Part 2: Count-Sketch

### Objective
Use the Count-Sketch algorithm to approximate \( S \) with reduced space requirements.

### Key Steps
1. **Hash Function Design**
   - Implement hash functions \( h(x) \) and \( s(x) \), where:
     - \( h(x) \) maps \( x \) to one of \( m \) counters.
     - \( s(x) \) is a sign function returning \( -1 \) or \( 1 \).

2. **Count-Sketch Implementation**
   - Maintain an array \( C \) of size \( m \) to store sketch values.
   - Update \( C[h(x)] \) based on the hash of \( x \) and its associated value \( d \).
   - Estimate \( S \) as \( X = \sum_{y=0}^{m-1} C[y]^2 \).

3. **Experimental Validation**
   - Generate data streams with varying properties.
   - Compare \( S \) approximated by Count-Sketch against the exact value.
   - Measure accuracy (Mean Square Error) and execution time for different values of \( m \).

---

## Data Stream Generation

Streams were generated using predefined functions in **C#**, with parameters:
- `n`: Total number of elements in the stream.
- `l`: Number of unique keys, \( 2^l \).
And temporarily saved into the src folder on execution. 

---

## Experimental Results

### Part 1: Hashing with Chaining
- Observed run times increase significantly with the number of unique keys.
- Multiply-Mod-Prime hashing performed better for large datasets due to reduced collisions.

### Part 2: Count-Sketch
- Approximation accuracy improved with larger \( m \), but at the cost of increased runtime.
- Even with smaller \( m \), Count-Sketch provided reasonable approximations with much lower space requirements compared to hashing with chaining.

---

## Conclusion

This project demonstrated the trade-offs between exact and approximate algorithms in terms of space and time efficiency. The Count-Sketch algorithm is particularly useful for processing large data streams with constrained memory while maintaining acceptable accuracy.