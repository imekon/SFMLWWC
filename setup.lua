function setup()
	print("Setting up WWC")

	vendor = {
		name = "vendor",
		strength = 22,
		armour = 20,
		lowest = 30
	}

	mouse = {
		name = "mouse",
		strength = 6,
		armour = 4,
		lowest = 15
	}

	rat = {
		name = "rat",
		strength = 8,
		armour = 6,
		lowest = 20
	}

	dog = {
		name = "dog",
		strength = 10,
		armour = 10,
		lowest = 20
	}

	vampire = {
		name = "vampire",
		strength = 12,
		armour = 12,
		lowest = 30
	}

	monsters = {
		vendor,
		mouse,
		rat,
		dog,
		vampire
	}

	return monsters
end


monsters = setup()

print(monsters[1].name..' '..#monsters)
