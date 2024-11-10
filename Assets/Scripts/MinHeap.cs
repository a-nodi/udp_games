using System;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<T> where T : IComparable<T>
{
    private List<T> heap;

    public MinHeap()
    {
        heap = new List<T>();
    }

    // Return the number of elements in the heap
    public int Count
    {
        get { return heap.Count; }
    }

    // Add an element to the heap
    public void Add(T element)
    {
        heap.Add(element);
        HeapifyUp(heap.Count - 1);
    }

    // Remove and return the minimum element from the heap
    public T RemoveMin()
    {
        if (heap.Count == 0) throw new InvalidOperationException("Heap is empty.");

        T minElement = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        if (heap.Count > 0)
        {
            HeapifyDown(0);
        }

        return minElement;
    }

    // Return the minimum element without removing it
    public T Peek()
    {
        if (heap.Count == 0) throw new InvalidOperationException("Heap is empty.");
        return heap[0];
    }

    // Helper method to maintain heap property on insertion
    private void HeapifyUp(int index)
    {
        int parentIndex = (index - 1) / 2;

        while (index > 0 && heap[index].CompareTo(heap[parentIndex]) < 0)
        {
            Swap(index, parentIndex);
            index = parentIndex;
            parentIndex = (index - 1) / 2;
        }
    }

    // Helper method to maintain heap property on removal
    private void HeapifyDown(int index)
    {
        int smallest = index;
        int leftChild = 2 * index + 1;
        int rightChild = 2 * index + 2;

        if (leftChild < heap.Count && heap[leftChild].CompareTo(heap[smallest]) < 0)
        {
            smallest = leftChild;
        }

        if (rightChild < heap.Count && heap[rightChild].CompareTo(heap[smallest]) < 0)
        {
            smallest = rightChild;
        }

        if (smallest != index)
        {
            Swap(index, smallest);
            HeapifyDown(smallest);
        }
    }

    // Swap two elements in the heap
    private void Swap(int index1, int index2)
    {
        T temp = heap[index1];
        heap[index1] = heap[index2];
        heap[index2] = temp;
    }
}
