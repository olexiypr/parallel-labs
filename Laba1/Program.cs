// See https://aka.ms/new-console-template for more information

using Laba1.CPUBound;
using Laba1.IOBound;
using Laba1.MemoryBound;

/*var cpuBound = new CPUBound();
cpuBound.TestFactorization();*/

/*var memoryBound = new MemoryBound();
memoryBound.TestTranspose();*/

//Directory.Delete("Files", true);

var ioBound = new IOBound();
ioBound.TestWordsCalculation();