param file := "beispiel_kwon.vrp";

set V := { read file as "<1n>" skip 7 use 16};
set E := { <i,j> in V * V with i < j };
set P[] := powerset(V);
set K := indexset(P);

param px[V] := read file as <1n> 2n skip 7 use 16;
param py[V] := read file as <1n> 3n skip 7 use 16;

defnumb dist(a,b) := sqrt(pow(px[a] - px[b],2) + pow(py[a] - py[b],2));

var x[E] binary;

minimize cost:
	sum <i,j> in E: x[i,j] * dist(i,j);
	
subto two_connected:
	forall <v> in V do (sum <v,j> in E: x[v,j]) + ( sum <i,v> in E: x{i,v]) == 2;