using System;

public interface NodeMutator
{
    void AddedToNode(Node node);
    void UpdateAffectedNode(Node node, float deltaTime);
    void RemovedFromNode(Node node);
}

