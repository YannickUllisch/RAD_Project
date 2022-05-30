from hashlib import new
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.colors import ListedColormap
import pandas as pd



# read the csv file to extract data

data = pd.read_csv('3medians.csv', delimiter=',')

plt.scatter(data['iteration'], data['estimate'], s=100, alpha=0.6, edgecolor='black', linewidth=1)
plt.scatter(data['iteration'], data['trueval'], edgecolor='red')
plt.title('Partition medians with m=2^22')
plt.xlabel('Estimate number')
plt.ylabel('Estimate')

plt.show()