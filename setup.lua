function setup()
	print("Setting up WWC")

	vendor = {
		name = "vendor",
		strength = 22,
		armour = 20,
		lowest = 30,
		highest = 0
	}

	mouse = {
		name = "mouse",
		strength = 6,
		armour = 4,
		lowest = 15,
		highest = 0
	}

	rat = {
		name = "rat",
		strength = 8,
		armour = 6,
		lowest = 20,
		highest = 0
	}

	dog = {
		name = "dog",
		strength = 10,
		armour = 10,
		lowest = 20,
		highest = 0
	}

	vampire = {
		name = "vampire",
		strength = 12,
		armour = 12,
		lowest = 30
		highest = 20
	}

	mimic = {
		name = "mimic",
		strength = 20,
		armour = 10,
		lowest = 40,
		highest = 20
	}

	monsters = {
		vendor,
		mouse,
		rat,
		dog,
		vampire,
		mimic
	}

	return monsters
end


monsters = setup()

