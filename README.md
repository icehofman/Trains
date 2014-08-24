Tains
=========
___


Requirements:
-------------

  - Microsoft Visual Studio 2013
  - .NET Framework 4.5.1
  - Nuget:
        NSubstitute
        NSubstituteAutoMocker
        NUnit
        
Version
----

1.0

Execute
--------------

C:\Users\ice...\Documents\Trains\Trains\Trains\bin\Debug>trains.exe default_data.txt

Result
--------------
```command
Output #1: 9
Output #2: 5
Output #3: 13
Output #4: 22
Output #5: NO SUCH ROUTE
Output #6: 2
Output #7: 3
Output #8: 9
Output #9: 9
Output #10: 7
 ==========
```

Test Result:
--------------
```command
#1 Graph:AB1 BC1 SpecifiedRoute:AB1 BC1 Valid Route:True
#1: --- Test passed ---
#2 Graph:AB1 BC1 SpecifiedRoute:AB0 BC1 Valid Route:False
#2: --- Test passed ---
#3 Graph:AB1 BC1 SpecifiedRoute:AB0 BD0 Valid Route:False
#3: --- Test passed ---
#4 Graph:AB1 BC1 SpecifiedRoute:AB0 BC0 CD0 Valid Route:False
#4: --- Test passed ---
#5 Graph:AB1 BC1 SpecifiedRoute:CB0 BC0 Valid Route:False
#5: --- Test passed ---
#6 Graph:AB1 BC1 CA1 AD1 DC1 SpecifiedRoute:AZ0 ZC0 Valid Route:True
#6: --- Test passed ---
#7 Graph:AB1 BC1 CA1 BD1 DA1 SpecifiedRoute:BZ0 ZA0 Valid Route:True
#7: --- Test passed ---
#8 Graph:AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7 SpecifiedRoute:AZ0 ZZ0 ZZ0 ZC0 Valid Route:True
#8: --- Test passed ---
#9 Graph:AB1 SpecifiedRoute:AB1 Valid Route:True
#9: --- Test passed ---
#10 Graph:AB1 SpecifiedRoute:AB0 Valid Route:True
#10: --- Test passed ---
#11 Graph:AB1 SpecifiedRoute:AC1 Valid Route:False
#11: --- Test passed ---
#12 Graph:AB3 SpecifiedRoute:AB2 Valid Route:False
#12: --- Test passed ---
#13 Graph:AB1 SpecifiedRoute:CB1 Valid Route:False
#13: --- Test passed ---
#14 Graph:AB1 SpecifiedRoute:AB0 BC0 Valid Route:False
#14: --- Test passed ---
#15 Graph:AB1 BC1 SpecifiedRoute:AB0 BC0 Valid Route:True
#15: --- Test passed ---
#16 Graph:AB1 BC1 SpecifiedRoute:BC1 Valid Route:True
#16: --- Test passed ---
```