# Hashbrown Tests

Runs a simple test involving two hashbrowns with the same salt. One generates the salt upon creation and saves it as expected, and the other loads it from the file that contains the generated salt. Two texts are hashed, one the same, and one different between both hashers.

The tester then verifies that the two texts that should match between both hashers indeed match, and the two that shouldn't do not.
