import torch
import torch.nn as nn
import torch.nn.functional as F

class QNetwork(nn.Module):

    def __init__(self, state_size = 12, action_size = 4, seed=42):

        super(QNetwork, self).__init__()
        self.seed = torch.manual_seed(seed)

        self.fc1 = nn.Linear(state_size, 32)
        self.relu1 = nn.ReLU()
        
        self.fc2 = nn.Linear(32, 64)
        self.relu2 = nn.ReLU()
        
        self.fc3 = nn.Linear(64, 6)
        self.relu3 = nn.ReLU()
        
        self.fc4 = nn.Linear(6, action_size)
        

    def forward(self, x):
        
        x = self.fc1(x)
        x = self.relu1(x)
        x = self.fc2(x)
        x = self.relu2(x)
        x = self.fc3(x)
        x = self.relu3(x)
        x = self.fc4(x)
        
        return x

class DuellingQNetwork(nn.Module):

    def __init__(self, state_size = 12, action_size = 4, seed=42):
        
        super(DuellingQNetwork, self).__init__()
        self.seed = torch.manual_seed(seed)
        
        self.fcx1 = nn.Linear(state_size, 32)
        self.relux1 = nn.ReLU()
        
        self.fcx2 = nn.Linear(32, 64)
        self.relux2 = nn.ReLU()
        
        self.fcx3 = nn.Linear(64, 128)
        self.relux3 = nn.ReLU()
        
        self.fcval = nn.Linear(128, 20)
        self.fcval2 = nn.Linear(20, 1)
        
        self.fcadv = nn.Linear(128, 20)
        self.fcadv2 = nn.Linear(20, action_size)

    def forward(self, x):    
        
        x = self.fcx1(x)
        x = self.relux1(x)
        x = self.fcx2(x)
        x = self.relux2(x)
        x = self.fcx3(x)
        x = self.relux3(x)
        
        advantage = self.fcadv(x)
        advantage = self.fcadv2(advantage)
        advantage = advantage - torch.mean(advantage, dim=-1, keepdim=True)
        
        value = F.relu(self.fcval(x))
        value = self.fcval2(value)

        return value + advantage